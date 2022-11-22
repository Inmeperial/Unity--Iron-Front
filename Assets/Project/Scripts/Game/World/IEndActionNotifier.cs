using System;
public interface IEndActionNotifier
{
    Action OnEndAction { get; set; }
}