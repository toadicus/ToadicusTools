// ToadicusTools
//
// TweakableAnimationWrapper.cs
//
// Copyright © 2014-2015, toadicus
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// 1. Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation and/or other
//    materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using KSP;
using System;
using ToadicusTools;
using UnityEngine;

namespace ToadicusTools
{
	public class AnimationWrapper
	{
		public Animation animation
		{
			get;
			private set;
		}

		public AnimationState animationState
		{
			get;
			private set;
		}

		public string stateName
		{
			get;
			private set;
		}

		public ModuleAnimateGeneric module
		{
			get;
			private set;
		}

		public PlayDirection startDirection
		{
			get;
			private set;
		}

		private AnimationWrapper(PlayDirection direction)
		{
			this.startDirection = direction;
		}

		public AnimationWrapper(Animation anim, string statename, PlayDirection direction) : this(direction)
		{
			this.animation = anim;
			this.animationState = anim[statename];
			this.stateName = statename;
		}

		public AnimationWrapper(ModuleAnimateGeneric mod, PlayDirection direction) : this(direction)
		{
			Animation[] partAnims = mod.part.FindModelAnimators(mod.animationName);

			if (partAnims.Length > 0)
			{
				this.module = mod;
				this.animation = partAnims[0];
				this.animationState = this.animation[mod.animationName];
				this.stateName = mod.animationName;
			}
		}

		public bool IsPlaying(string statename)
		{
			return (this.animation != null && this.animation.IsPlaying(statename));
		}

		public bool IsPlaying()
		{
			return this.IsPlaying(this.stateName);
		}

		public void Stop(string statename)
		{
			if (this.animation != null)
			{
				this.animation.Stop(statename);
			}
		}

		public void Stop()
		{
			this.Stop(this.stateName);
		}

		public void SkipTo(PlayPosition position)
		{
			if (this.animation == null || this.animationState == null)
			{
				return;
			}

			switch (position)
			{
				case PlayPosition.End:
					switch (this.startDirection)
					{
						case PlayDirection.Forward:
							if (module != null)
								this.module.animTime = 1f;
							this.animationState.normalizedTime = 1f;
							this.animationState.speed = 1f;
							this.animation.Play(this.stateName);
							break;
						case PlayDirection.Backward:
							if (module != null)
								this.module.animTime = 0f;
							this.animationState.normalizedTime = 0f;
							this.animationState.speed = -1f;
							this.animation.Play(this.stateName);
							break;
					}
					break;
				case PlayPosition.Beginning:
					switch (this.startDirection)
					{
						case PlayDirection.Backward:
							if (module != null)
								this.module.animTime = 1f;
							this.animationState.normalizedTime = 1f;
							this.animationState.speed = 1f;
							this.animation.Play(this.stateName);
							break;
						case PlayDirection.Forward:
							if (module != null)
								this.module.animTime = 0f;
							this.animationState.normalizedTime = 0f;
							this.animationState.speed = -1f;
							this.animation.Play(this.stateName);
							break;
					}
					break;
			}
		}

		public override string ToString()
		{
			return string.Format(
				"[AnimationWrapper: animation={0}, animationState={1}, stateName={2}, module={3}, startDirection={4}]",
				animation == null ? "null" : animation.ToString(),
				animationState == null ? "null" : animationState.ToString(),
				stateName,
				module == null ? "null" : module.ToString(),
				startDirection
			);
		}
	}
}
