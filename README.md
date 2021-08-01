# LoadingBar class
Simple command-line loading bar to display progress towards a given task's completion. 
Implements the IDisposable interface and is expected to be used withing a "using" block.

## Usage
``` 
using (LoadingBar bar = new LoadingBar())
{
    while (bar.Progress < 1.0)
    {
        await Task.Delay(rng.Next(10, 25));         // Simulate work being done
        bar.Progress += 0.002545;                   // Simulate progress being made towards final goal
    }
}
```

