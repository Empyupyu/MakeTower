using Source.Scripts.DragAndDrop;

public interface IPlacementStrategy
{
    public void Place(IDragAndDrop dragAndDrop);
}