using System.ComponentModel;
using System.Linq;

namespace Api.Configurations
{
    public static class ValidationOptions
    {
        public static void Config()
        {
            FluentValidation.ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
            {
                if (member != null)
                {
                    var attr = member.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                        .Cast<DisplayNameAttribute>()
                        .SingleOrDefault();
                    if (attr == null)
                    {
                        return member.Name;
                    }

                    return attr.DisplayName;
                }
                return null;
            };
        }

    }
}
