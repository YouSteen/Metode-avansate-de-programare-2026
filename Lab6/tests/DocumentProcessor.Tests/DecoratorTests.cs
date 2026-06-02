using DocumentProcessor.Core.Adapter;
using DocumentProcessor.Core.Decorators;
using DocumentProcessor.Core.Exceptions;
using DocumentProcessor.Core.Interfaces;
using NUnit.Framework;

namespace DocumentProcessor.Tests;

[TestFixture]
public class DecoratorTests
{
  [Test]
  public void ValidationDecorator_Throws_ForEmptyTitle()
  {
    IDocumentParser inner = new EmptyTitleParser();
    var parser = new ValidationDocumentParser(inner);

    Assert.Throws<ValidationException>(() => parser.Parse("<ignored>"));
  }

  [Test]
  public void CachingDecorator_CallsInnerParserOnce_ForSameContent()
  {
    var counting = new CountingParser();
    var parser = new CachingDocumentParser(counting);
    const string content = "acelasi continut de test";

    parser.Parse(content);
    parser.Parse(content);

    Assert.That(counting.ParseCount, Is.EqualTo(1));
  }

  [Test]
  public void CombinedDecorators_WorkInChain()
  {
    IDocumentParser pipeline = new LoggingDocumentParser(
      new ValidationDocumentParser(
        new XmlParserAdapter()));

    var doc = pipeline.Parse("""
      <document>
        <title>Valid</title>
        <content>1234567890 ok</content>
      </document>
      """);

    Assert.That(doc.Title, Is.EqualTo("Valid"));
  }

  private class EmptyTitleParser : IDocumentParser
  {
    public DocumentProcessor.Core.Models.Document Parse(string content) =>
      new() { Title = "   ", Content = "1234567890 text" };
  }
}
