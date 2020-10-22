var gulp = require('gulp');
var gp_clean = require('gulp-clean');
var gp_concat = require('gulp-concat');
var gp_sourcemaps = require('gulp-sourcemaps');
var gp_uglify = require('gulp-uglify');
var gp_ngmin = require('gulp-ngmin');
var ng_annotate = require('gulp-ng-annotate');

var srcPaths = {
    app: [
        'app/**/*.js',
    ]
};

var destPaths = {
    app: 'app/build/app',
    js: 'app/build/js'
};

gulp.task('app_build', ['app_clean'], function(){
    return gulp.src(srcPaths.app)
        .pipe(gp_sourcemaps.init())
        .pipe(ng_annotate())
        .pipe(gp_uglify({mangle: false}))
        .pipe(gp_concat('all-js.min.js'))
        .pipe(gp_sourcemaps.write('/'))
        .pipe(gulp.dest(destPaths.js));
})

gulp.task('app_clean', function () {
    return gulp.src(destPaths.app + "*", { read: false })
        .pipe(gp_clean({ force: true }));
});

gulp.task('js_clean', function () {
    return gulp.src(destPaths.js + "*", { read: false })
        .pipe(gp_clean({ force: true }));
});

gulp.task('cleanup', ['app_clean', 'js_clean']);