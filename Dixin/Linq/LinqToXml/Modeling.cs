﻿namespace Dixin.Linq.LinqToXml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
#if !NETFX
    using System.IO;
#endif
    using System.Linq;
#if !NETFX
    using System.Net.Http;
#endif
    using System.Xml;
    using System.Xml.Linq;

    internal static partial class Modeling
    {
        internal static void CreateAndSerialize()
        {
            XNamespace @namespace = "https://weblogs.asp.net/dixin";
            XElement rss = new XElement(
                "rss",
                new XAttribute("version", "2.0"),
                new XAttribute(XNamespace.Xmlns + "dixin", @namespace),
                new XElement(
                    "channel",
                    new XElement(
                        "item", // Implicitly converted to XName.
                        new XElement("title", "LINQ via C#"),
                        new XElement("link", "https://weblogs.asp.net/dixin/linq-via-csharp"),
                        new XElement(
                            "description",
                            XElement.Parse("<p>This is a tutorial of LINQ and functional programming. Hope it helps.</p>")),
                        new XElement("pubDate", new DateTime(2009, 9, 7).ToString("r")),
                        new XElement(
                            "guid",
                            new XAttribute("isPermaLink", "true"), // "isPermaLink" is implicitly converted to XName.
                            "https://weblogs.asp.net/dixin/linq-via-csharp"),
                        new XElement("category", "C#"),
                        new XElement("category", "LINQ"),
                        new XComment("Comment."),
                        new XElement(
                            @namespace + "source",
                            "https://github.com/Dixin/CodeSnippets/tree/master/Dixin/Linq"))));
            // Serialize XDocument to string.
            rss.ToString().WriteLine();
        }
    }

    internal static partial class Modeling
    {
        internal static void Construction()
        {
            XDeclaration declaration = new XDeclaration("1.0", null, "no");
            declaration.WriteLine(); // <?xml version="1.0" standalone="no"?>

            XDocumentType documentType = new XDocumentType("html", null, null, null);
            documentType.WriteLine(); // <!DOCTYPE html >

            XText text = new XText("<p>text</p>");
            text.WriteLine(); // &lt;p&gt;text&lt;/p&gt;

            XCData cData = new XCData("cdata");
            cData.WriteLine(); // <![CDATA[cdata]]>

            XProcessingInstruction processingInstruction = new XProcessingInstruction(
                "xml-stylesheet", @"type=""text/xsl"" href=""Style.xsl""");
            processingInstruction.WriteLine(); // <?xml-stylesheet type="text/xsl" href="Style.xsl"?>
        }

        internal static void Name()
        {
            XName attributeName1 = "isPermaLink"; // Implicitly convert string to XName.
            XName attributeName2 = XName.Get("isPermaLink");
            XName attributeName3 = "IsPermaLink";
            object.ReferenceEquals(attributeName1, attributeName2).WriteLine(); // True
            (attributeName1 == attributeName2).WriteLine(); // True
            (attributeName1 != attributeName3).WriteLine(); // True
        }

        internal static void Namespace()
        {
            XNamespace namespace1 = "http://www.w3.org/XML/1998/namespace"; // Implicitly convert string to XNamespace.
            XNamespace namespace2 = XNamespace.Xml;
            XNamespace namespace3 = XNamespace.Get("http://www.w3.org/2000/xmlns/");
            (namespace1 == namespace2).WriteLine(); // True
            (namespace1 != namespace3).WriteLine(); // True

            XNamespace @namespace = "https://weblogs.asp.net/dixin";
            XName name = @namespace + "localName"; // + operator.
            name.WriteLine(); // {https://weblogs.asp.net/dixin}localName
            XElement element = new XElement(name, new XAttribute(XNamespace.Xmlns + "dixin", @namespace)); // + operator.
            element.WriteLine(); // <dixin:localName xmlns:dixin="https://weblogs.asp.net/dixin" />
        }

        internal static void Element()
        {
            XElement pubDateElement = XElement.Parse("<pubDate>Mon, 07 Sep 2009 00:00:00 GMT</pubDate>");
            DateTime pubDate = (DateTime)pubDateElement;
            pubDate.WriteLine(); // 9/7/2009 12:00:00 AM
        }

        internal static void Attribute()
        {
            XName name = "isPermaLink";
            XAttribute isPermaLinkAttribute = new XAttribute(name, "true");
            bool isPermaLink = (bool)isPermaLinkAttribute;
            isPermaLink.WriteLine(); // True
        }

        internal static void Node()
        {
            XDocument document = XDocument.Parse("<element></element>");
            XElement element1 = new XElement("element", null); // <element />
            XElement element2 = new XElement("element", string.Empty); // <element></element>
            XNode.DeepEquals(document.Root, element1).WriteLine(); // False
            XNode.DeepEquals(document.Root, element2).WriteLine(); // True
        }

        internal static XDocument LoadXDocument(string url)
        {
#if NETFX
            return XDocument.Load(url);
#else
            using (HttpClient httpClient = new HttpClient())
            using (Stream downloadStream = httpClient.GetStreamAsync(url).Result)
            {
                return XDocument.Load(downloadStream);
            }
#endif
        }

        internal static XElement LoadXElement(string url)
        {
#if NETFX
            return XElement.Load(url);
#else
            using (HttpClient httpClient = new HttpClient())
            using (Stream downloadStream = httpClient.GetStreamAsync(url).Result)
            {
                return XElement.Load(downloadStream);
            }
#endif
        }

        internal static XmlReader CreateReader(string url)
        {
#if NETFX
            return XmlReader.Create(url);
#else
            using (HttpClient httpClient = new HttpClient())
            {
                Stream downloadStream = httpClient.GetStreamAsync(url).Result;
                return XmlReader.Create(downloadStream);
            }
#endif
        }

        internal static void Read()
        {
            using (XmlReader reader = CreateReader("https://weblogs.asp.net/dixin/rss"))
            {
                reader.MoveToContent();
                XNode node = XNode.ReadFrom(reader);
            }

            XElement element1 = XElement.Parse("<html><head></head><body></body></html>");
            XElement element2 = LoadXElement("https://weblogs.asp.net/dixin/rss");

            XDocument document1 = XDocument.Parse("<html><head></head><body></body></html>");
            XDocument document2 = LoadXDocument("https://microsoft.com"); // Succeed.
            XDocument document3 = LoadXDocument("https://asp.net"); // Fail.
            // System.Xml.XmlException: The 'ul' start tag on line 68 position 116 does not match the end tag of 'div'. Line 154, position 109.
        }

        internal static IEnumerable<XElement> RssItems(string rssUrl)
        {
            using (XmlReader reader = CreateReader(rssUrl))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("item", StringComparison.Ordinal))
                    {
                        yield return (XElement)XNode.ReadFrom(reader);
                    }
                }
            }
        }

#if NETFX
        internal static void Write()
        {
            XDocument document1 = LoadXDocument("https://weblogs.asp.net/dixin/rss");
            document1.Save(Path.GetTempFileName());

            XElement element1 = new XElement("element", string.Empty);
            using (XmlTextWriter writer = new XmlTextWriter(Console.Out))
            {
                element1.WriteTo(writer); // <element></element>
            }

            XDocument document2 = new XDocument();
            using (XmlWriter writer = document2.CreateWriter())
            {
                element1.WriteTo(writer);
            }
            document2.WriteLine(); // <element></element>

            XElement element2 = new XElement("element", string.Empty);
            using (XmlWriter writer = element2.CreateWriter())
            {
                writer.WriteStartElement("child");
                writer.WriteAttributeString("attribute", "value");
                writer.WriteString("text");
                writer.WriteEndElement();
            }
            element2.ToString(SaveOptions.DisableFormatting).WriteLine();
            // <element><child attribute="value">text</child></element>
        }
#endif

        internal static void XNodeToString()
        {
            XDocument document = XDocument.Parse(
                "<root xmlns:prefix='namespace'><element xmlns:prefix='namespace' /></root>");
            document.ToString(SaveOptions.None).WriteLine(); // Equivalent to document.ToString().
            // <root xmlns:prefix="namespace">
            //  <element xmlns:prefix="namespace" />
            // </root>
            document.ToString(SaveOptions.DisableFormatting).WriteLine();
            // <root xmlns:prefix="namespace"><element xmlns:prefix="namespace" /></root>
            document.ToString(SaveOptions.OmitDuplicateNamespaces).WriteLine();
            // <root xmlns:prefix="namespace">
            //  <element />
            // </root>
        }

        internal static void StreamingElement()
        {
            Func<IEnumerable<XElement>> getChildElements = () => Enumerable
                .Range(0, 5)
                .Do(value => value.WriteLine())
                .Select(value => new XElement("child", value));

            XElement immediate1 = new XElement("parent", getChildElements()); // 0 1 2 3 4.

            XStreamingElement deferred1 = new XStreamingElement("parent", getChildElements());
            deferred1.ToString(SaveOptions.DisableFormatting).WriteLine();
            // 0 1 2 3 4 <parent><child>0</child><child>1</child><child>2</child><child>3</child><child>4</child></parent>

            XElement immediate2 = new XElement("parent", immediate1.Elements());
            XStreamingElement deferred2 = new XStreamingElement("parent", immediate1.Elements());
            immediate1.RemoveAll();
            immediate2.ToString(SaveOptions.DisableFormatting).WriteLine();
            // <parent><child>0</child><child>1</child><child>2</child><child>3</child><child>4</child></parent>
            deferred2.WriteLine(); // <parent />
        }
    }
}
