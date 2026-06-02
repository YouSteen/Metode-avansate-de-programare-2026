using DocumentProcessor.Core.Facade;
using DocumentProcessor.Core.Services;
using NUnit.Framework;

namespace DocumentProcessor.Tests;

[TestFixture]
public class FacadeTests
{
  [Test]
  public void Process_ReturnsFailure_ForInvalidContent_WithoutThrowing()
  {
    var tempFile = Path.Combine(Path.GetTempPath(), $"invalid_{Guid.NewGuid():N}.xml");
    File.WriteAllText(tempFile, """
      <document>
        <title></title>
        <content>scurt</content>
      </document>
      """);

    try
    {
      var facade = new DocumentProcessingFacade(
        new InternalDocumentTransformer(),
        new FileDocumentStorage(Path.Combine(Path.GetTempPath(), "docproc_test")),
        enableLogging: false);

      var result = facade.Process(tempFile);

      Assert.That(result.IsSuccess, Is.False);
      Assert.That(result.Message, Is.Not.Empty);
    }
    finally
    {
      if (File.Exists(tempFile))
        File.Delete(tempFile);
    }
  }

  [Test]
  public void Process_ReturnsSuccess_ForValidJsonFile()
  {
    var tempFile = Path.Combine(Path.GetTempPath(), $"valid_{Guid.NewGuid():N}.json");
    var outputDir = Path.Combine(Path.GetTempPath(), $"docproc_ok_{Guid.NewGuid():N}");
    File.WriteAllText(tempFile, """
      {"title":"Factura","content":"1234567890 linii factura"}
      """);

    try
    {
      var facade = new DocumentProcessingFacade(
        enableLogging: false,
        storage: new FileDocumentStorage(outputDir));

      var result = facade.Process(tempFile);

      Assert.That(result.IsSuccess, Is.True);
      Assert.That(result.SavedPath, Is.Not.Null.And.Not.Empty);
      Assert.That(File.Exists(result.SavedPath!), Is.True);
    }
    finally
    {
      if (File.Exists(tempFile))
        File.Delete(tempFile);
    }
  }
}
