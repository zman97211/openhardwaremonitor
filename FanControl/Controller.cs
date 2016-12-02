using System.Linq;
using OpenHardwareMonitor.Hardware;

namespace FanControl
{
    class Controller
    {
        private readonly IHardware _gpu;
        private readonly ISensor _fanControl;
        private readonly ISensor _tempSensor;

        public Controller()
        {
            var computer = new Computer();
            computer.Open();

            computer.GPUEnabled = true;
            computer.FanControllerEnabled = true;

            _gpu = computer.Hardware.First(h => h.HardwareType == HardwareType.GpuAti);
            _fanControl = _gpu.Sensors.First(s => s.SensorType == SensorType.Control && s.Name == "GPU Fan");
            _tempSensor = _gpu.Sensors.First(s => s.SensorType == SensorType.Temperature);
        }

        public float Speed
        {
            get { return GetSpeed(); }
            set { SetSpeed(value); }
        }

        public float Temperature
        {
            get { return GetTemperature(); }
            set { SetTemperature(value); }
        }

        private void SetTemperature(float value)
        {
            _fanControl.Control.SetSoftware(value);
        }

        private float GetTemperature()
        {
            var v = _fanControl.Value;
            return v.GetValueOrDefault(float.NaN);
        }

        private void SetSpeed(float value)
        {
            _fanControl.Control.SetSoftware(value);
        }

        private float GetSpeed()
        {
            _gpu.Update();
            var v = _tempSensor.Value;
            return v.GetValueOrDefault(float.NaN);
        }
    }
}
