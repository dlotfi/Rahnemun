using System;

namespace Rahnemun.CaptchaContracts
{
    public class CaptchaRouteModel
    {
        public Guid Id { get; set; }

        public static implicit operator CaptchaRouteModel(Guid value)
        {
            return new CaptchaRouteModel { Id = value };
        }
    }
}