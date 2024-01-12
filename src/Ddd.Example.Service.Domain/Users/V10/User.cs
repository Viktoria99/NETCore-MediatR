using System.Diagnostics.CodeAnalysis;

namespace Ddd.Example.Service.Domain.Users.V10
{

    [ExcludeFromCodeCoverage]
    public class User
    {

        public int Id { get; set; }

        public string Login { get; set; }

        public string ProfileEQ { get; set; }
    }
}
