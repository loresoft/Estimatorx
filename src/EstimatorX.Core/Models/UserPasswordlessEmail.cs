namespace EstimatorX.Core.Models
{
    public class UserPasswordlessEmail : EmailModelBase
    {
        public int ExpireMinutes { get; set; } = 15;
    }

}
