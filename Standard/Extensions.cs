using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Metarract.GameLib.Standard;
public static class Extensions {
  private const uint FNV1_A_OFFSET_BIAS = 2166136261;
  private const uint FNV1_A_PRIME = 16777619;
  
  public static (TB, TA) Reverse<TA, TB>(this (TA, TB) tuple) => (tuple.Item2, tuple.Item1);

  public static T DeepCopy<T>(this T obj) where T : ISerializable {
    using var memStream = new MemoryStream();
    var serializer = new XmlSerializer(typeof(T));
    var xmlReader = XmlReader.Create(new StreamReader(memStream));
    serializer.Serialize(memStream, obj);
    memStream.Position = 0;
    
    return (T)serializer.Deserialize(xmlReader);
  }
  
  public static int ComputeFNV1aHash(this string value) {
    uint hash = FNV1_A_OFFSET_BIAS;
    foreach (char c in value) hash = (hash ^ c) * FNV1_A_PRIME;
    
    return unchecked((int)hash);
  }
}
