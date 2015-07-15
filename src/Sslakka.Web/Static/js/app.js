﻿/**
 * File   : app.js
 * Author : Herdy Handoko
 * Created: 2015/07/09
 * License:
 *
 *   Copyright (c) 2015 Sslakka and its contributors
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *     http: *www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 */
(function () {
    'use strict';

    angular.module('sslakka', [
        'ngAnimate',
        'ngRoute',
        'mgcrea.ngStrap',
        'sslakka.controllers'
    ]).config(['$routeProvider',

        /**
         * Configure Angular routing
         *
         * @param $routeProvider the routing provider.
         */
        function ($routeProvider) {
            $routeProvider
                .when('/', {
                    templateUrl: '/app/index',
                    controller: 'HomeController'
                });
        }

    ]);

})();