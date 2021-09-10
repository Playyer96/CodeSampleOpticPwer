using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DreamHouseStudios.SofasaLogistica {
    public class ReferenceGetter : InputField {
        public InputField Selected;

        public override void OnSelect (BaseEventData eventData) {
            base.OnSelect (eventData);
            Selected = this;
            UIKeyboard.instance.actualInputField = Selected;
            UIKeyboard.instance.word = Selected.text;
        }
    }
}