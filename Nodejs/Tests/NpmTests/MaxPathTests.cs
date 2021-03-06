﻿//*********************************************************//
//    Copyright (c) Microsoft. All rights reserved.
//    
//    Apache 2.0 License
//    
//    You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//    
//    Unless required by applicable law or agreed to in writing, software 
//    distributed under the License is distributed on an "AS IS" BASIS, 
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 
//    implied. See the License for the specific language governing 
//    permissions and limitations under the License.
//
//*********************************************************//

using System.Threading;
using Microsoft.NodejsTools.Npm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NpmTests {

    [TestClass]
    public class MaxPathTests : AbstractPackageJsonTests {
        
        [TestMethod, Priority(0), TestCategory("AppVeyorIgnore")]
        public void InstallUninstallMaxPathGlobalModule() {
            using (var manager = new TemporaryFileManager()) {
                var rootDir = FilesystemPackageJsonTestHelpers.CreateRootPackage(manager, PkgSimple);
                var controller = NpmControllerFactory.Create(rootDir, string.Empty);

                using (var commander = controller.CreateNpmCommander()) {
                    commander.InstallPackageByVersionAsync("yo", "^1.2.0", DependencyType.Standard, false).Wait();
                }

                Assert.IsNotNull(controller.RootPackage, "Cannot retrieve packages after install");
                Assert.IsTrue(controller.RootPackage.Modules.Contains("yo"), "Package failed to install");

                using (var commander = controller.CreateNpmCommander()) {
                    commander.UninstallPackageAsync("yo").Wait();
                }

                // Command has completed, but need to wait for all files/folders to be deleted.
                Thread.Sleep(5000);

                Assert.IsNotNull(controller.RootPackage, "Cannot retrieve packages after uninstall");
                Assert.IsFalse(controller.RootPackage.Modules.Contains("yo"), "Package failed to uninstall");
            }
        }
    }
}
