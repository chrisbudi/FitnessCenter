using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Class
{

    public sealed class StatusAction
    {

        private readonly String name;

        public static readonly StatusAction Freeze = new StatusAction(1, "Freeze");
        public static readonly StatusAction Upgrade = new StatusAction(2, "Upgrade");
        public static readonly StatusAction Transfer = new StatusAction(3, "Transfer");

        private StatusAction(int id, String name)
        {
            this.name = name;
            this.Id = id;
        }

        public static IEnumerable<string> ToList()
        {
            return typeof(StatusAction).GetFields()
                .Select(field => field.Name);
        }

        public override String ToString()
        {
            return name;
        }

        public int Id { get; }
    }
}
