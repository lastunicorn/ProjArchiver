// ProjArchiver
// Copyright (C) 2011 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace DustInTheWind.ProjArchiver.Tests
{
    class XmlAsserter
    {
        private readonly XPathNavigator navigator;

        public XmlAsserter(Stream stream)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);
            navigator = xmlDocument.CreateNavigator();
        }

        public void NodeValue(string nodeXPath, string expectedValue)
        {
            XPathNodeIterator nodeIterator = navigator.Select(nodeXPath);

            if (!nodeIterator.MoveNext())
            {
                string errorMessage = string.Format("Invalid description file. Node '{0}' not found.", nodeXPath);
                throw new Exception(errorMessage);
            }

            string actualValue = nodeIterator.Current.Value;

            if (actualValue != expectedValue)
            {
                string errorMessage = string.Format("Expected '{0}', but found '{1}", expectedValue, actualValue);
                throw new Exception(errorMessage);
            }
        }
    }
}
