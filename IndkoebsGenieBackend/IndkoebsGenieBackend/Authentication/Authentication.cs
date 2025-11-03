namespace IndkoebsGenieBackend.Authentication
{
    public class Authentication
    {
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
        public class AllowAnnonymousAttribute : Attribute { }
    }
}
