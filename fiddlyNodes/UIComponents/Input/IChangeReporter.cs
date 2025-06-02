namespace fiddlyNodes;

public interface IChangeReporter<T>
{
	public Action<T> OnChange { get; set; }
}