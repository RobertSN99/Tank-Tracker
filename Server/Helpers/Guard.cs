using Microsoft.AspNetCore.Identity;

namespace Server.Helper
{
    public static class Guard
    {
        public static IdentityResult AgainstNullOrEmpty(string value, string fieldName)
        {
            return string.IsNullOrEmpty(value)
                ? IdentityResult.Failed(new IdentityError { Description = $"{fieldName} cannot be null or empty." })
                : IdentityResult.Success;
        }

        public static IdentityResult AgainstCondition(bool condition, string errorMessage)
        {
            return condition
                ? IdentityResult.Failed(new IdentityError { Description = errorMessage })
                : IdentityResult.Success;
        }

        public static IdentityResult AgainstNull(object obj, string fieldName)
        {
            return obj == null
                ? IdentityResult.Failed(new IdentityError { Description = $"{fieldName} cannot be null." })
                : IdentityResult.Success;
        }
    }
}
