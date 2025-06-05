using fiddlyNodes;
namespace fiddlyTests;

public class Tests
{
	private static NodeFinder _nf;
	[SetUp]
	public void Setup()
	{
		_nf = new NodeFinder();
	}

	
	[Test]
	public void EqualStringsNoEdits ()
	{
		Assert.That(0, Is.EqualTo(FuzzyUtility.DLDistance("test", "test")));
	}
	
	[Test]
	public void Additions ()
	{
		Assert.That(1, Is.EqualTo(FuzzyUtility.DLDistance("test", "tests")));
		Assert.That(1, Is.EqualTo(FuzzyUtility.DLDistance("test", "stest")));
		Assert.That(2, Is.EqualTo(FuzzyUtility.DLDistance("test", "mytest")));
		Assert.That(7, Is.EqualTo(FuzzyUtility.DLDistance("test", "mycrazytest")));
	}
	
	[Test]
	public void AdditionsPrependAndAppend ()
	{
		Assert.That(9, Is.EqualTo(FuzzyUtility.DLDistance("test", "mytestiscrazy")));
	}
	
	[Test]
	public void AdditionOfRepeatedCharacters ()
	{
		Assert.That(1, Is.EqualTo(FuzzyUtility.DLDistance("test", "teest")));
	}
	
	[Test]
	public void Deletion ()
	{
		Assert.That(1, Is.EqualTo(FuzzyUtility.DLDistance("test", "tst")));
	}
	
	[Test]
	public void Transposition ()
	{
		Assert.That(1, Is.EqualTo(FuzzyUtility.DLDistance("test", "tset")));
	}
	
	[Test]
	public void AdditionWithTransposition ()
	{
		Assert.That(2, Is.EqualTo(FuzzyUtility.DLDistance("test", "tsets")));
	}
	
	[Test]
	public void TranspositionOfRepeatedCharacters ()
	{
		Assert.That(1, Is.EqualTo(FuzzyUtility.DLDistance("banana", "banaan")));
		Assert.That(1, Is.EqualTo(FuzzyUtility.DLDistance("banana", "abnana")));
		Assert.That(2, Is.EqualTo(FuzzyUtility.DLDistance("banana", "baanaa")));
	}
	
	[Test]
	public void EmptyStringsNoEdits ()
	{
		Assert.That(0, Is.EqualTo(FuzzyUtility.DLDistance("", "")));
	}

	[Test]
	public void SearchNodeList()
	{
		string q = "cir";
		var r = _nf.FindMatchingItems(q, 1);
		Assert.That("Circle", Is.EqualTo(r[0].name));

		q = "flo";
		r = _nf.FindMatchingItems(q);
		Assert.That("Float", Is.EqualTo(r[0].name));

		q = "trnslt";
		r = _nf.FindMatchingItems(q);
		Assert.That("Translate", Is.EqualTo(r[0].name));
	}
}