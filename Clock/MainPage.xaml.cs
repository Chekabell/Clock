
namespace Clock;


public class GridNumber
{
    private Grid _grid;
    private List<BoxView> _cells;

    public GridNumber(Grid grid)
    {
        _grid = grid;
        _cells = new List<BoxView>(15);
        FullGridTale();
    }
    
    public enum CodeNumber : ushort
    {
        Zero = 0b0_111_101_101_101_111,
        One = 0b0_001_011_001_001_001,
        Two = 0b0_010_101_001_010_111,
        Three = 0b0_110_001_010_001_110,
        Four = 0b0_101_101_111_001_001,
        Five = 0b0_111_100_111_001_111,
        Six = 0b0_111_100_111_101_111,
        Seven = 0b0_111_001_001_001_001,
        Eight = 0b0_111_101_010_101_111,
        Nine = 0b0_111_101_111_001_111
    }
    
    public static CodeNumber ToCodeNumber(int number)
    {
        switch (number)
        {
            case 0: return CodeNumber.Zero;
            case 1: return CodeNumber.One;
            case 2: return CodeNumber.Two;
            case 3: return CodeNumber.Three;
            case 4: return CodeNumber.Four;
            case 5: return CodeNumber.Five;
            case 6: return CodeNumber.Six;
            case 7: return CodeNumber.Seven;
            case 8: return CodeNumber.Eight;
            case 9: return CodeNumber.Nine;
        }
        return CodeNumber.Zero;
    }
    
    private void FullGridTale()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                BoxView box = new BoxView{Color = Colors.Red};
                _grid.Add(box, j, i);
                _cells.Add(box);
            }
        }
    }

    public void Update(CodeNumber codeNumber)
    {
        ushort control = (ushort)codeNumber;
        ushort mask = 0b0_000_000_000_000_001;
        for (int i = 14; i >= 0; i--)
        {  
            _cells[i].IsVisible = Convert.ToBoolean(mask & control);
            mask <<= 1;
        }
    }
}

public partial class MainPage : ContentPage
{
    private List<GridNumber> _numbers = new(6);
    
    public MainPage()
    {
        InitializeComponent();
        _numbers.Add(new GridNumber(GridLoverSec));
        _numbers.Add(new GridNumber(GridHighestSec));
        _numbers.Add(new GridNumber(GridLoverMinute));
        _numbers.Add(new GridNumber(GridHighestMinute));
        _numbers.Add(new GridNumber(GridLoverHour));
        _numbers.Add(new GridNumber(GridHighestHour));
        StartTimerTick();
    }

    private async void StartTimerTick()
    {
        var time = DateTime.Now;
        UpdateTimer(time);
        var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await periodicTimer.WaitForNextTickAsync())
        {
            time = time.AddSeconds(1);
            UpdateTimer(time);
        }
    }

    private void UpdateTimer(DateTime time)
    {
        _numbers[5].Update(GridNumber.ToCodeNumber(time.Second%10));
        _numbers[4].Update(GridNumber.ToCodeNumber(time.Second/10));
        _numbers[3].Update(GridNumber.ToCodeNumber(time.Minute%10));
        _numbers[2].Update(GridNumber.ToCodeNumber(time.Minute/10));
        _numbers[1].Update(GridNumber.ToCodeNumber(time.Hour%10));
        _numbers[0].Update(GridNumber.ToCodeNumber(time.Hour/10));
    }

}