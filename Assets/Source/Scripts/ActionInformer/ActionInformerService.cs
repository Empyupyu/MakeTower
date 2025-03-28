using Source.Scripts.Localization;

namespace Source.Scripts.ActionInfromer
{
    public class ActionInformerService
    {
        private readonly ActionInformerView _actionInformerView;
        private readonly LocalizationService _localizationService;

        public ActionInformerService(ActionInformerView actionInformerView, LocalizationService localizationService)
        {
            _actionInformerView = actionInformerView;
            _localizationService = localizationService;
        }
        
        public void ShowActionByTag(string tag)
        {
            _actionInformerView.InfoText.text =  _localizationService.LocalizationByTag(tag);;
        }
    }
}