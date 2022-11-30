using System;
public interface IEndActionNotifier
{
    Action<IEndActionNotifier> OnEndAction { get; set; }
}