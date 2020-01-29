using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Xml;

namespace UnitTestProject3
{
    [TestClass]
    public class GetCountries
    {
        [TestMethod]
        public void Get_Countries()
        {

            var client = new RestClient("http://oc-svr-at1/Fltpro_Automation_main/API/v1/");

            var request = new RestRequest("countries", Method.GET);
            request.AddHeader("Content-Type", "application/xml");
            request.AddHeader("X-ExternalRequest-ID", "D1AD98CB-5C2B-419A-AB7C-5C93B570E3CA");
            request.AddHeader("X-ExternalSystem-ID", "5");
            request.AddHeader("X-Date", "Monday, 24 June 2019");
            request.AddHeader("X-AuthenticatedPerson", "d300a2c2b5394a13807eea513f40c0c2");
            request.AddHeader("X-Hash", "SmfOoxSL5UVKas5A76QzObzabt/WjovRcwisGej6Mion8ydnNo9NhsEk3STHmsQC1rAkyjwp71U2nR4RLm6dZ2FjYTljYjVmMDQ4ODQzOTM2ZjgyMmE5ZDJmZTY2ZTVh");
            request.AddHeader("Authorization", "Bearer eyJ0eXAiOiAiSldUIn0.eyAicGVvcGxlSWQiOiAiMSIsImV4cCI6IDE1NjMzNDA5ODcgfQ.W/uMWrJxlP9kfOLWdDd/c4GQyJisUrpxILrC3X7zoXFPqwMoOzdFPZIrIXYIymtTLrX3J3evMmyKbHJj9VUcI+mTgu+YkuOFud645rCX672L76+X47q15K6K47SVxajmqLvnhZjqqZvmoYDihYTjhIPmmqXskrfripLvv73mtazni4bjjK7kt5Hvv73qm6Htgabki6Lvv73jtKjjvbjjgZ/grp/svZPkupDiv4Lipa3Sp+eJg+Oil+eNveWLt+uQveODv+W2vOShmuWHjOOavO6NmuKMge6SqOy8kueTlOO7ueKMs+Cou+ujt+OQvuqLo+mZhue5jOiWpOyanOOmh+KIruC5tuiuu+i9nuKpg+G1g+OjlA==");


            var response = client.Execute(request);
            var content = client.Execute(request).Content;

            Console.WriteLine(content.Length);
            Console.WriteLine(response.ResponseStatus);

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Headers.Count);

            var doc = new XmlDocument();

            doc.LoadXml(response.Content);

            string Countryname = doc.GetElementsByTagName("CountryName")[1].InnerText;

            Assert.IsNotNull(doc.GetElementsByTagName("CountryName")[1].InnerText);

            Console.WriteLine("CountryName" + Countryname);



        }
    }
}
