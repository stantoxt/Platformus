﻿// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Platformus.Routing.EndpointResolvers;

namespace Platformus.Routing.Actions
{
  public class AddEndpointResolverAction : IConfigureServicesAction
  {
    public int Priority => 4000;

    public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
    {
      services.AddScoped(typeof(IEndpointResolver), typeof(DefaultEndpointResolver));
    }
  }
}