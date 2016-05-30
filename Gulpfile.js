var gulp = require('gulp');
var babel = require('gulp-babel');
var sourcemaps = require("gulp-sourcemaps");
var concat = require("gulp-concat");
var less = require("gulp-less");
var path = require("path");
var flatten = require("gulp-flatten");

gulp.task('babel', function() {
	return gulp.src(["./HotelWeb/**/*.js", "!./HotelWeb/dist/**"])
		.pipe(sourcemaps.init())
		.pipe(babel({ presets: ['es2015'] }))
		.pipe(sourcemaps.write("."))
		.pipe(flatten())
		.pipe(gulp.dest("./HotelWeb/dist"));
});

gulp.task("less", function() {
	return gulp.src("./HotelWeb/Content/Less/main.less")
	.pipe(less({
		paths: ["./HotelWeb" ]
	}))
	.pipe(gulp.dest("./HotelWeb/dist"));
});

gulp.task("copy-images", function() {
	return gulp.src("./HotelWeb/Content/Images/**.*")
	.pipe(gulp.dest("./HotelWeb/dist"))
})

gulp.task('libs', function(){
    return gulp.src([
        'node_modules/systemjs/dist/system.js',
        'node_modules/babel-polyfill/dist/polyfill.js'])
        .pipe(gulp.dest('./HotelWeb/dist/libs'));
});

gulp.task("default", ["babel", "less", "copy-images", "libs"]);

var jsWatcher = gulp.watch(["HotelWeb/**/*.js", "!HotelWeb/dist/**"], ["babel"]);
jsWatcher.on("change", function(event) {
	console.log("File " + event.path + " was changed");
});

var lessWatcher = gulp.watch("HotelWeb/**/*.less", ["less"]);
lessWatcher.on("change", function(event) {
	console.log("File " + event.path + " was changed");
});

var imageWatcher = gulp.watch("HotelWeb/Content/Images/*.*", ["copy-images"]);
imageWatcher.on("change", function(event) {
	console.log("File " + event.path + " was changed");
});


