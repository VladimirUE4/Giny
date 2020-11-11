////////////////////////////////////////////////////////////////////////////////
//
//  Licensed to the Apache Software Foundation (ASF) under one or more
//  contributor license agreements.  See the NOTICE file distributed with
//  this work for additional information regarding copyright ownership.
//  The ASF licenses this file to You under the Apache License, Version 2.0
//  (the "License"); you may not use this file except in compliance with
//  the License.  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//
////////////////////////////////////////////////////////////////////////////////

package mx.automation.delegates.controls
{
	import flash.display.DisplayObject;
	
	import mx.automation.Automation; 
	import mx.automation.delegates.core.UIComponentAutomationImpl;
	import mx.controls.VideoDisplay
		
		[Mixin]
		/**
		 * 
		 *  Defines methods and properties required to perform instrumentation for the 
		 *  VideoDisplay control.
		 * 
		 *  @see mx.controls.VideoDisplay 
		 *
		 *  
		 *  @langversion 3.0
		 *  @playerversion Flash 9
		 *  @playerversion AIR 1.1
		 *  @productversion Flex 3
		 */
		public class VideoDisplayAutomationImpl extends UIComponentAutomationImpl
		{
			include "../../../core/Version.as";
			
			//--------------------------------------------------------------------------
			//
			//  Class methods
			//
			//--------------------------------------------------------------------------
			
			/**
			 *  Registers the delegate class for a component class with automation manager.
			 *  
			 *  @param root The SystemManger of the application.
			 *  
			 *  @langversion 3.0
			 *  @playerversion Flash 9
			 *  @playerversion AIR 1.1
			 *  @productversion Flex 3
			 */
			public static function init(root:DisplayObject):void
			{
				Automation.registerDelegateClass(VideoDisplay, VideoDisplayAutomationImpl);
			}   
			
			//--------------------------------------------------------------------------
			//
			//  Constructor
			//
			//--------------------------------------------------------------------------
			
			/**
			 *  Constructor.
			 * @param obj VideoDisplay object to be automated.     
			 *  
			 *  @langversion 3.0
			 *  @playerversion Flash 9
			 *  @playerversion AIR 1.1
			 *  @productversion Flex 3
			 */
			public function VideoDisplayAutomationImpl(obj:VideoDisplay)
			{
				super(obj);
				
				recordClick = true;
			}
			
		}
		
}