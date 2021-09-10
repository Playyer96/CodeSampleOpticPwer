using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public abstract class Tutorials : MonoBehaviour
    {
        #region Components

        [Space(10f), Header("Components")] public DetectControllersScriptable detectControllers = null;

        public CanvasManager canvasManager;
        public SpectraUISettings settings;

        #endregion

        #region Unity Functions

        public virtual void Start()
        {
        }

        #endregion

        #region Functions

        public virtual void Step(string checklist, string item)
        {
            do
            {
                DoWhile();
            } while (Checklist.Get(checklist, item));
        }

        public virtual void DoWhile()
        {
        }

        public enum EppStepToStep
        {
            Controles,
            ModoEntrenamiento,
            EquipateLosEpp,
            FaltanElementos,
            SiguienteModulo
        }

        public enum ReceptionStepToStep
        {
            Bienvenida,
            FotografiaLaCaja,
            CortarCintasCaja,
            LogueateEnElPocket,
            EscaneaLaCaja,
        }

        public enum LocationStepToStep
        {
        }

        public enum PickingStepToStep
        {
        }

        public enum PackingStepToStep
        {
        }

        #endregion
    }
}