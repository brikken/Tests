define("myclass", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var default_1 = /** @class */ (function () {
        function default_1() {
            this.val = 10;
        }
        default_1.prototype.GetValue = function () {
            // this is returning a value
            return this.val;
        };
        return default_1;
    }());
    exports.default = default_1;
});
define("myapp", ["require", "exports", "myclass"], function (require, exports, myclass_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    // this is my object
    var myObj = new myclass_1.default();
    // here I print the boring value
    console.log(myObj.GetValue());
});
//# sourceMappingURL=bundle.js.map