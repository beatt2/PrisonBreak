﻿using System.Collections.Generic;
using TerrainGen.Data;
using UnityEngine;

namespace TerrainGen
{
// ReSharper disable RedundantAssignment

    public static class MeshGenerator
    {
	
        public static MeshData GenerateTerrainMesh(float[,] heightMap,MeshSettings meshSettings, int levelOfDetail)
        {

            int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
            int borderedSize = heightMap.GetLength(0);
            int meshSize = borderedSize - 2 * meshSimplificationIncrement;
            int meshSizeUnsimplified = borderedSize - 2;

            float topLeftX = (meshSizeUnsimplified - 1) / -2f;
            float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

		
            int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;
	

            MeshData meshData = new MeshData(verticesPerLine, meshSettings.UseFlatShading);
            int[,] vertexIndicesMap = new int[borderedSize,borderedSize];
            int meshVertexIndex = 0;
            int borderVertexIndex = -1;

            for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
            {
                for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
                {
                    bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;
                    if (isBorderVertex)
                    {
                        vertexIndicesMap[x, y] = borderVertexIndex;
                        borderVertexIndex--;
                    }
                    else
                    {
                        vertexIndicesMap[x, y] = meshVertexIndex;
                        meshVertexIndex++;
                    }

                }
            }

            for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
            {
                for (int x = 0; x < borderedSize; x+= meshSimplificationIncrement)
                {
                    int vertexIndex = vertexIndicesMap[x, y];

                    Vector2 percent = new Vector2((x - meshSimplificationIncrement) / (float)meshSize, (y - meshSimplificationIncrement) / (float)meshSize);
                    float height = heightMap[x, y];
                    Vector3 vertexPosition = new Vector3((topLeftX + percent.x * meshSizeUnsimplified) * meshSettings.MeshScale, height, (topLeftZ -percent.y *meshSizeUnsimplified) * meshSettings.MeshScale);
				
                    meshData.AddVertex(vertexPosition, percent, vertexIndex);

                    if (x < borderedSize - 1 && y < borderedSize - 1)
                    {
                        int a = vertexIndicesMap[x, y];
                        int b = vertexIndicesMap[x + meshSimplificationIncrement, y];
                        int c = vertexIndicesMap[x, y + meshSimplificationIncrement];
                        int d = vertexIndicesMap[x + meshSimplificationIncrement, y + meshSimplificationIncrement];
                        meshData.AddTriangle(a,d,c);
                        meshData.AddTriangle(d,a,b);

                    }

                    vertexIndex++;
                }
            }

            meshData.ProcessMesh();
            return meshData;
        }



	
    }

    public class MeshData
    {
        private Vector3[] _vertices;
        private readonly int [] _triangles;

        private Vector2[] _uvs;
        private Vector3[] _bakedNormals;

        private readonly Vector3[] _borderVertices;
        private readonly int[] _borderTriangles;

	

        private int _triangleIndex;
        private int _borderTriangleIndex;
        private readonly bool _useFlataShading;



        public MeshData(int verticesPerLine, bool useFlatShading)
        {
            _useFlataShading = useFlatShading;
            _vertices = new Vector3[verticesPerLine * verticesPerLine];
            _uvs = new Vector2[verticesPerLine * verticesPerLine];
            _triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];

            _borderVertices = new Vector3[verticesPerLine * 4 + 4];
            _borderTriangles = new int[24 * verticesPerLine];

        }

        public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
        {
            if (vertexIndex < 0)
            {
                _borderVertices[-vertexIndex - 1] = vertexPosition;
            }
            else
            {
                _vertices[vertexIndex] = vertexPosition;
                _uvs[vertexIndex] = uv;
            }
        }

        public List<Vector3> GetVertices(ObjectSpawnData data)
        {
            List<Vector3> localVerts = new List<Vector3>();
            int lastRecord = 0;
            float maxHeight = float.MinValue;
            float minHeight = float.MaxValue;
            for (int i = 0; i < _vertices.Length; i++)
            {
                if (_vertices[i].y > maxHeight)
                {
                    maxHeight = _vertices[i].y;
                }

                if (_vertices[i].y < minHeight)
                {
                    minHeight = _vertices[i].y;
                }
            }




            for (int j = 0; j < _vertices.Length && localVerts.Count < data.AmountToSpawn; j++)
            {
                if (_vertices[j].y > data.StartHeight && _vertices[j].y < data.EndHeight)
                {

                    if (lastRecord < j)
                    {
                        localVerts.Add(_vertices[j]);
                        lastRecord = j + data.MaxDistance;
                    }
                }
            }

            return localVerts;
        }



        public void AddTriangle(int a, int b, int c)
        {
            if (a < 0 || b < 0 || c < 0)
            {
                _borderTriangles[_borderTriangleIndex] = a;
                _borderTriangles[_borderTriangleIndex + 1] = b;
                _borderTriangles[_borderTriangleIndex + 2] = c;
                _borderTriangleIndex += 3;
            }
            else
            {
                _triangles[_triangleIndex] = a;
                _triangles[_triangleIndex + 1] = b;
                _triangles[_triangleIndex + 2] = c;
                _triangleIndex += 3;
            }
	 

        }

        private Vector3[] CalculateNormals()
        {
            Vector3[] vertexNormals = new Vector3[_vertices.Length];
            int triangleCount = _triangles.Length / 3;
            for (int i = 0; i < triangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = _triangles[normalTriangleIndex];
                int vertexIndexB = _triangles[normalTriangleIndex + 1];
                int vertexIndexC = _triangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
                vertexNormals[vertexIndexA] += triangleNormal;
                vertexNormals[vertexIndexB] += triangleNormal;
                vertexNormals[vertexIndexC] += triangleNormal;
            }

            int borderTriangleCount = _borderTriangles.Length / 3;
            for (int i = 0; i < borderTriangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = _triangles[normalTriangleIndex];
                int vertexIndexB = _triangles[normalTriangleIndex + 1];
                int vertexIndexC = _triangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);

                if (vertexIndexA >= 0)
                {
                    vertexNormals[vertexIndexA] += triangleNormal;
                }

                if (vertexIndexB >= 0)
                {
                    vertexNormals[vertexIndexB] += triangleNormal;
                }

                if (vertexIndexC >= 0)
                {
                    vertexNormals[vertexIndexC] += triangleNormal;
                }
		   
            }

            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i].Normalize();
            }

		
            return vertexNormals;
        }


        private Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
        {
            Vector3 pointA =(indexA < 0)? _borderVertices[-indexA -1]: _vertices[indexA];
            Vector3 pointB = (indexB < 0) ? _borderVertices[-indexB - 1] : _vertices[indexB];
            Vector3 pointC = (indexC < 0) ? _borderVertices[-indexC - 1] : _vertices[indexC];


            Vector3 sideAB = pointB - pointA;
            Vector3 sideAC = pointC - pointA;

            return Vector3.Cross(sideAB, sideAC).normalized;
        }

        public void ProcessMesh()
        {
            if (_useFlataShading)
            {
                FlatShading();
            }
            else
            {
                BakeNormals();
            }
        }

        private void BakeNormals()
        {
            _bakedNormals = CalculateNormals();
        }

        private void FlatShading()
        {
            Vector3[] flatShadedVertices = new Vector3[_triangles.Length];
            Vector2[] flatShadedUvs = new Vector2[_triangles.Length];

            for (int i = 0; i < _triangles.Length; i++)
            {
                flatShadedVertices[i] = _vertices[_triangles[i]];
                flatShadedUvs[i] = _uvs[_triangles[i]];
                _triangles[i] = i;
            }

            _vertices = flatShadedVertices;
            _uvs = flatShadedUvs;

        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh
            {
                vertices = _vertices,
                triangles = _triangles,
                uv = _uvs
            };
            if (_useFlataShading)
            {
                mesh.RecalculateNormals();
            }
            else
            {
                mesh.normals = _bakedNormals;
            }
		
            return mesh;


        }
    }
}