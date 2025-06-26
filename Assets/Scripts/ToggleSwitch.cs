public class ToggleSwitch<T>
{
    private T first, second;
    private bool isSwitched;

    public ToggleSwitch(T a, T b)
    {
        first = a; second = b;
        isSwitched = false;
    }

    public void Toggle() => isSwitched = !isSwitched;
    public T GetValue() => isSwitched ? second : first;
    public T GetInverseValue() => isSwitched ? first : second;
}