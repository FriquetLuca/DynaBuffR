# DynaBuffR

> A pseudo-generic way to convert a class into a binary array, taking only it's fields values. It can also re-create the object only from a binary array representation.


DynaBuffR handle all native types such as int, char, string, long, ...
What should be implemented:
- Array support
- Struct / Class support
- Maybe more..

```Cs
using DynaBuffR;
public class TestProps {
    /* Disable warning for this empty constructor */
#pragma warning disable CS8618
    public TestProps() { }
#pragma warning restore CS8618
    private string firstName;
    private string lastName;
    private int age;

    public string GetName { get => firstName; } // Shouldn't be taken into account, it's not a field
    public TestProps(string firstName, string lastName, int age) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.age = age;
    }
    public static bool isSame(TestProps a, TestProps b) {
        return a.firstName == b.firstName
        && a.lastName == b.lastName
        && a.age == b.age;
    }
}
class Program {
    static void Main() {
        int offset = 0;
        TestProps test = new TestProps("John", "Doe", 25);
        byte[] buffer = Extractor.ExtractBufferObject<TestProps>(test);
        TestProps testCloneFromBuffer = Injector.CreateInstance<TestProps>(ref offset, buffer);
        Console.WriteLine($"The object contain the same properties: {TestProps.isSame(test, testCloneFromBuffer)}");
    }
}
```
