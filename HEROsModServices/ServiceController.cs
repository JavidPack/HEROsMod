using System;
using System.Collections.Generic;

namespace HEROsMod.HEROsModServices
{
	internal class ServiceController
	{
		public delegate void ServiceEventHandler(HEROsModService service);

		public event ServiceEventHandler ServiceAdded;

		public event ServiceEventHandler ServiceRemoved;

		private List<HEROsModService> _services;

		/// <summary>
		/// HEROsMod Services laoded into the controller
		/// </summary>
		public List<HEROsModService> Services
		{
			get { return _services; }
		}

		public ServiceController()
		{
			_services = new List<HEROsModService>();
			HEROsModNetwork.LoginService.MyGroupChanged += LoginService_MyGroupChanged;
		}

		private void LoginService_MyGroupChanged(object sender, EventArgs e)
		{
			MyGroupChanged();
		}

		public void MyGroupChanged()
		{
			if (HEROsModNetwork.LoginService.MyGroup != null)
			{
				foreach (HEROsModService service in _services)
				{
					//ErrorLogger.Log("MyGroupChanged for " + service.Name);
					service.MyGroupUpdated();
				}
				// These not bound to a service because of complicated UI coupling
				foreach (var item in HEROsMod.instance.crossModGroupUpdated)
				{
					item.Value.Invoke(HEROsModNetwork.LoginService.MyGroup.HasPermission(item.Key));
				}
				ServiceRemoved(null);
			}
		}

		/// <summary>
		/// Add a HEROsModService to the ServiceController
		/// </summary>
		/// <param name="service">Service to add</param>
		public void AddService(HEROsModService service)
		{
			_services.Add(service);
			ServiceAdded?.Invoke(service);
		}

		/// <summary>
		/// Remove a HEROsModService from the ServiceController
		/// </summary>
		/// <param name="service">Service to Remove</param>
		public void RemoveService(HEROsModService service)
		{
			service.Destroy();
			_services.Remove(service);
			ServiceRemoved?.Invoke(service);
		}

		/// <summary>
		/// Remove all HEROsModServices from the ServiceController
		/// </summary>
		public void RemoveAllServices()
		{
			while (_services.Count > 0)
			{
				RemoveService(_services[0]);
			}
		}

		internal void ServiceRemovedCall()
		{
			ServiceRemoved(null);
		}
	}
}