﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;

namespace Platformus.Barebone.Backend.Controllers
{
  public abstract class ControllerBase : Platformus.Barebone.Controllers.ControllerBase
  {
    public ControllerBase(IStorage storage)
      : base(storage)
    {
    }
  }
}