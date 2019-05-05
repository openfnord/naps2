﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NAPS2.Serialization;
using Xunit;

namespace NAPS2.Sdk.Tests.Serialization
{
    public class XmlSerializerTests
    {
        private static readonly XNamespace Xsi = "http://www.w3.org/2001/XMLSchema-instance";

        [Fact]
        public void SerializePoco()
        {
            var original = new Poco { Str = "Hello world", Int = 42 };
            var serializer = new XmlSerializer<Poco>();
            var doc = serializer.SerializeToXDocument(original);
            Assert.NotNull(doc.Root);
            Assert.Equal("Poco", doc.Root.Name);
            Assert.Equal(2, doc.Root.Elements().Count());
            var strEl = doc.Root.Element("Str");
            Assert.NotNull(strEl);
            var intEl = doc.Root.Element("Int");
            Assert.NotNull(intEl);
            Assert.Equal("Hello world", strEl.Value);
            Assert.Equal("42", intEl.Value);

            var copy = serializer.DeserializeFromXDocument(doc);
            Assert.Equal("Hello world", copy.Str);
            Assert.Equal(42, copy.Int);
        }

        [Fact]
        public void SerializePrivateSetter()
        {
            var original = new PrivateSetter("Hello");
            var serializer = new XmlSerializer<PrivateSetter>();
            var doc = serializer.SerializeToXDocument(original);
            Assert.NotNull(doc.Root);
            Assert.Equal("PrivateSetter", doc.Root.Name);
            Assert.Single(doc.Root.Elements());
            var strEl = doc.Root.Element("Str");
            Assert.NotNull(strEl);
            Assert.Equal("Hello", strEl.Value);

            var copy = serializer.DeserializeFromXDocument(doc);
            Assert.Equal("Hello", copy.Str);
        }

        [Fact]
        public void SerializeNested()
        {
            var original = new NestedPoco
            {
                Child = new Poco
                {
                    Str = "Test"
                }
            };
            var serializer = new XmlSerializer<NestedPoco>();
            var doc = serializer.SerializeToXDocument(original);
            Assert.NotNull(doc.Root);
            var childEl = doc.Root.Element("Child");
            Assert.NotNull(childEl);
            var strEl = childEl.Element("Str");
            Assert.NotNull(strEl);
            Assert.Equal("Test", strEl.Value);

            var copy = serializer.DeserializeFromXDocument(doc);
            Assert.Equal("Test", copy?.Child.Str);
        }

        [Fact]
        public void SerializeNull()
        {
            var original = new Poco { Str = null };
            var serializer = new XmlSerializer<Poco>();
            var doc = serializer.SerializeToXDocument(original);
            Assert.NotNull(doc.Root);
            var xsiAttr = doc.Root.Attribute(XNamespace.Xmlns + "xsi");
            Assert.NotNull(xsiAttr);
            Assert.Equal(xsiAttr.Value, Xsi.NamespaceName);
            var childEl = doc.Root.Element("Str");
            Assert.NotNull(childEl);
            Assert.True(childEl.IsEmpty);
            var nullAttr = childEl.Attribute(Xsi + "nil");
            Assert.NotNull(nullAttr);
            Assert.Equal("true", nullAttr.Value);

            var copy = serializer.DeserializeFromXDocument(doc);
            Assert.Null(copy.Str);
        }

        [Fact]
        public void SerializeList()
        {
            var original = new List<Poco> { new Poco { Str = "Hello" }, new Poco { Str = "World" } };
            var serializer = new XmlSerializer<List<Poco>>();
            var doc = serializer.SerializeToXDocument(original);
            Assert.NotNull(doc.Root);
            Assert.Equal("ArrayOfPoco", doc.Root.Name);
            Assert.Equal(2, doc.Root.Elements().Count());
            var first = doc.Root.Elements().First();
            Assert.NotNull(first);
            Assert.Equal("Poco", first.Name);
            Assert.Equal("Hello", first.Element("Str")?.Value);
            var last = doc.Root.Elements().Last();
            Assert.NotNull(last);
            Assert.Equal("Poco", last.Name);
            Assert.Equal("World", last.Element("Str")?.Value);

            var copy = serializer.DeserializeFromXDocument(doc);
            Assert.Equal(2, copy.Count);
            Assert.Equal("Hello", copy[0].Str);
            Assert.Equal("World", copy[1].Str);
        }

        [Fact]
        public void SerializeArray()
        {
            var original = new[] { new Poco { Str = "Hello" }, new Poco { Str = "World" } };
            var serializer = new XmlSerializer<Poco[]>();
            var doc = serializer.SerializeToXDocument(original);
            Assert.NotNull(doc.Root);
            Assert.Equal("ArrayOfPoco", doc.Root.Name);
            Assert.Equal(2, doc.Root.Elements().Count());
            var first = doc.Root.Elements().First();
            Assert.NotNull(first);
            Assert.Equal("Poco", first.Name);
            Assert.Equal("Hello", first.Element("Str")?.Value);
            var last = doc.Root.Elements().Last();
            Assert.NotNull(last);
            Assert.Equal("Poco", last.Name);
            Assert.Equal("World", last.Element("Str")?.Value);

            var copy = serializer.DeserializeFromXDocument(doc);
            Assert.Equal(2, copy.Length);
            Assert.Equal("Hello", copy[0].Str);
            Assert.Equal("World", copy[1].Str);
        }

        [Fact]
        public void SerializeSubclass()
        {
            var original = new PocoSubtype { Str = "Hello", Int = 42, Bool = true };
            var serializer = new XmlSerializer<Poco>();
            var doc = serializer.SerializeToXDocument(original);
            Assert.NotNull(doc.Root);
            Assert.Equal("Poco", doc.Root.Name);
            var typeAttr = doc.Root.Attribute(Xsi + "type");
            Assert.NotNull(typeAttr);
            Assert.Equal("PocoSubtype", typeAttr.Value);
            Assert.Equal(3, doc.Root.Elements().Count());

            var copy = serializer.DeserializeFromXDocument(doc);
            var copySubtype = Assert.IsType<PocoSubtype>(copy);
            Assert.Equal("Hello", copySubtype.Str);
            Assert.Equal(42, copySubtype.Int);
            Assert.True(copySubtype.Bool);
        }

        // TODO: Custom serialization
        // TODO: Ordering

        private class NestedPoco
        {
            public Poco Child { get; set; }
        }

        private class Poco
        {
            public string Str { get; set; }

            public int Int { get; set; }
        }

        private class PocoSubtype : Poco
        {
            public bool Bool { get; set; }
        }

        private class PocoTypes : CustomXmlTypes<Poco>
        {
            protected override Type[] GetKnownTypes() => new[] { typeof(PocoSubtype) };
        }

        private class PrivateSetter
        {
            public PrivateSetter()
            {
            }

            public PrivateSetter(string str)
            {
                Str = str;
            }

            public string Str { get; private set; }
        }
    }
}
