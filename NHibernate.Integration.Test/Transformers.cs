﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver;
using DynamicMapResolver.Impl;
using NHibernate.Integration.Test.Poco;
using NHibernate.Transform;
using NUnit.Framework;

namespace NHibernate.Integration.Test
{
    [TestFixture]
    public class Transformers
    {
        public void Test1()
        {
            var aa = FactoryMapper.DynamicResolutionMapper<Person, Student>();

            DelegationTransformer<Person, Student> tr1 = new DelegationTransformer<Person, Student>(person => aa.Map(person));
        }

        
    }

}