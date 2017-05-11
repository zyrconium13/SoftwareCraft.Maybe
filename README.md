# SoftwareCraft.Maybe
SoftwareCraft.Maybe represents the OOP implementation of a functional concept. Instead of returning null, return a Maybe, and the code will be much cleaner and the intent much clearer.
## Examples
### Basic Usage
This code:
```c#
public interface IRepository
{
	Entity GetById(int id);
}
```
would turn into this:
```c#
public interface IRepository
{
	Maybe<Entity> GetById(int id);
}
```
### Extracting the Value
If you want to retrieve the value from a Maybe you can call the ValueOrDefault method. It requires you to provide a default value, in case the Maybe does not contain anything. This is required because otherwise it would have to return null, and null is exactly what we want to avoid.
```c#
var maybe = new Maybe<object>(); // build an empty Maybe
var value = maybe.ValueOrDefault(new object());
```
In this case you would get the provided default object, because the Maybe is empty.
### Mapping
The map method allows providing different Action callbacks for the case when the Maybe contains or does not contain a value.
```c#
var maybe = new Maybe<object>();
maybe.Map(
	(o) => DoSomethingWith(o)),
	() => DoSomethingWhenNoValue());
```
DoSomethingWith and DoSomethingWhenNoValue are two Actions that get called depending on whether the Maybe contains a value or not. The first Action gets called with the value, whereas the second Action is called without any arguments.
The Map method has an overload that allow you to specify only the Action callback to call only when there is a value, thus ignoring the case when there is no value present.
### Wrapping an Existing Method
The FromResult method allow wrapping the output of an existing method that may already return an instance of a reference type or null. The FromResult method basically transforms the output into a Maybe.
```c#
Func<object> func = () => new object();

Maybe<object> maybe = Maybe<object>.FromResult(func);
```
You would get a Maybe that contains the new object as its value.
