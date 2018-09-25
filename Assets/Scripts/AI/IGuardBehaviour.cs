namespace AI
{
    public interface IGuardBehaviour
    {
        GuardManager.GuardState GetEnum();
        float GetDamage();
    }
}
