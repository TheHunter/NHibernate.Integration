using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Integration.Test.Poco
{
    public class Person
    {
        public string Name { get; set; }

        public string Surname { get; set; }

    }

    public class Student
        : Person
    {
        public string Matricola { get; set; }
    }
}
