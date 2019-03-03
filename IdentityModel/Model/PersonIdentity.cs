using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityModel.Model
{
    public class PersonIdentity
    {
        public class Person
        {
            public int PersonID { get; set; }

            public string PNama { get; set; }

            public string PIdentitas { get; set; }

            public string PEmail { get; set; }

            public string PHP1 { get; set; }

            public string PPinBB { get; set; }

            public string Id { get; set; }

        }

        public class Member
        {
            public int MemberID { get; set; }

            public string MemberNo { get; set; }

            public DateTime MMulai { get; set; }

            public string MRFID { get; set; }
        }

        public class BackOffice
        {

            public string BOID { get; set; }

            public bool StatusBOID { get; set; }

            public string BRFID { get; set; }

            public int PosisiID { get; set; }

        }
    }
}