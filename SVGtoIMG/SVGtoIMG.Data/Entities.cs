﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ContextGenerator.ttinclude code generation file.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using SVGtoIMG.Data;

namespace SVGtoIMG.Data	
{
	public partial class Entities : OpenAccessContext, IEntitiesUnitOfWork
	{
		private static string connectionStringName = @"Connection";
			
		private static BackendConfiguration backend = GetBackendConfiguration();
				
		private static MetadataSource metadataSource = XmlMetadataSource.FromAssemblyResource("Entities.rlinq");
		
		public Entities()
			:base(connectionStringName, backend, metadataSource)
		{ }
		
		public Entities(string connection)
			:base(connection, backend, metadataSource)
		{ }
		
		public Entities(BackendConfiguration backendConfiguration)
			:base(connectionStringName, backendConfiguration, metadataSource)
		{ }
			
		public Entities(string connection, MetadataSource metadataSource)
			:base(connection, backend, metadataSource)
		{ }
		
		public Entities(string connection, BackendConfiguration backendConfiguration, MetadataSource metadataSource)
			:base(connection, backendConfiguration, metadataSource)
		{ }
			
		public IQueryable<TransaccionTicketsImpresion> TransaccionTicketsImpresions 
		{
			get
			{
				return this.GetAll<TransaccionTicketsImpresion>();
			}
		}
		
		public IQueryable<TransaccionPullTicketsImpresion> TransaccionPullTicketsImpresions 
		{
			get
			{
				return this.GetAll<TransaccionPullTicketsImpresion>();
			}
		}
		
		public IQueryable<Ticket> Tickets 
		{
			get
			{
				return this.GetAll<Ticket>();
			}
		}
		
		public static BackendConfiguration GetBackendConfiguration()
		{
			BackendConfiguration backend = new BackendConfiguration();
			backend.Backend = "MsSql";
			backend.ProviderName = "System.Data.SqlClient";
		
			CustomizeBackendConfiguration(ref backend);
		
			return backend;
		}
		
		/// <summary>
		/// Allows you to customize the BackendConfiguration of Entities.
		/// </summary>
		/// <param name="config">The BackendConfiguration of Entities.</param>
		static partial void CustomizeBackendConfiguration(ref BackendConfiguration config);
		
	}
	
	public interface IEntitiesUnitOfWork : IUnitOfWork
	{
		IQueryable<TransaccionTicketsImpresion> TransaccionTicketsImpresions
		{
			get;
		}
		IQueryable<TransaccionPullTicketsImpresion> TransaccionPullTicketsImpresions
		{
			get;
		}
		IQueryable<Ticket> Tickets
		{
			get;
		}
	}
}
#pragma warning restore 1591