namespace Calendar.BLL.Abstract.Converter
{
    public interface IConverterService<out T, in TK>
    {
        T Convert(TK attr);
    }
}