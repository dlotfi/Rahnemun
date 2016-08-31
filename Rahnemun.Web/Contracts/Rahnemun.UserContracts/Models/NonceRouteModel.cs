namespace Rahnemun.UserContracts
{
    public class NonceRouteModel
    {
        public string Nonce { get; set; }

        public static implicit operator NonceRouteModel(string value)
        {
            return new NonceRouteModel { Nonce = value };
        }
    }
}