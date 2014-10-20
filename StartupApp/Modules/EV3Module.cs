﻿using System;
using System.Diagnostics;
using Nancy;
using MonoBrickFirmware;
using MonoBrickFirmware.Movement;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;

namespace StartupApp.WebServer
{
	
	public class EV3Module : NancyModule
	{
		//static readonly Logger _logger = new Logger(typeof(MotorsModule));
		// MonoBrickFirmware.Movement.Motor[] motors = {new Motor(MotorPort.OutA), new Motor(MotorPort.OutB), new Motor(MotorPort.OutC), new Motor(MotorPort.OutD)};
		public static IList<MotorModel> Motors = new List<MotorModel>()
	    {
	        new MotorModel {Port = "A", Position = 0, Speed = 0},
			new MotorModel {Port = "B", Position = 0, Speed = 0},
			new MotorModel {Port = "C", Position = 0, Speed = 0},
			new MotorModel {Port = "D", Position = 0, Speed = 0},

	   	};

	    public dynamic Model = new ExpandoObject();

	    private int MotorIndexToInt (string index)
		{
			int motorIndex = 0;
			if (int.TryParse (index, out motorIndex)) 
			{
				motorIndex = motorIndex -1;//convert to array index
			} 
			else 
			{
				char charIndex = Convert.ToChar(index);
				motorIndex = (int) charIndex;
				if(motorIndex >= 97 && motorIndex < 101)//a-d
					motorIndex = motorIndex -97;
				if(motorIndex >= 65 && motorIndex < 69) //A-D
					motorIndex = motorIndex -65;
			}
			return motorIndex;
		}

	    public EV3Module()
	    {
	        Get["/"] = _ =>
	        {
	            Model.Motors = Motors;
				return View["index", Model];
	        };
			Get	["/status"] = parameter =>
			{
			    return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(Motors);//JSON
			};
			Post	["/motor/{index}/break/"] = parameter =>
			{
				Motors[MotorIndexToInt((string)parameter.index)].Break();
				return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(Motors);//JSON
			};
			Post	["/motor/{index}/off/"] = parameter =>
			{
				Motors[MotorIndexToInt((string)parameter.index)].Stop();
				return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(Motors);//JSON
			};
			Post	["/motor/{index}/forward/{speed}"] = parameter =>
			{
				Motors[MotorIndexToInt((string)parameter.index)].Forward((int)parameter.speed);
				return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(Motors);//JSON
			};
			Post	["/motor/{index}/reverse/{speed}"] = parameter =>
			{
				Motors[MotorIndexToInt((string)parameter.index)].Reverse((int)parameter.speed);
				return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(Motors);//JSON
			};

			Post	["/motor/{index}/move/{speed}/{steps}"] = parameter =>
			{
				Motors[MotorIndexToInt((string)parameter.index)].Move((int)parameter.speed, (int)parameter.steps);
				return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(Motors);//JSON
			};

			Post	["/motor/{index}/reset"] = parameter =>
			{
				Motors[MotorIndexToInt((string)parameter.index)].Reset();
				return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(Motors);//JSON
			};
	    }
	}
}
