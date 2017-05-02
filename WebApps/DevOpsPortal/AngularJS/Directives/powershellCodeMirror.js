PowershellApp.directive('codeMirror', ['$timeout', function($timeout) {
  return {
      restrict: 'E',
      replace: true,
      templateUrl: '/Content/Templates/codeMirror.html',
      scope: {
        container: '=',
        theme: '@',
        lineNumbers: '@',
        //tabMode: 'shift',
        //readOnly: 'nocursor',
        //matchBrackets: 'true'
      },
      link: function(scope, element, attrs) {
        var textarea = element.find('textarea')[0];
        var showLineNumbers = scope.lineNumbers === 'true' ? true : false;
        var codeMirrorConfig = {
          lineNumbers: showLineNumbers,
          mode: scope.syntax || 'powershell',
          matchBrackets: true,
          theme: scope.theme || 'default',
          value: scope.content || ''
        };
        scope.syntax = 'powershell';
        var myCodeMirror = CodeMirror.fromTextArea(textarea, codeMirrorConfig);

        // If we have content coming from an ajax call or otherwise, asign it
        var unwatch = scope.$watch('container.content', function(oldValue, newValue) {
          if(oldValue !== '') {
            myCodeMirror.setValue(oldValue);
            unwatch();
          }
          else if(oldValue === '' && newValue === '') {
            unwatch();
          }
        });

        // Change the mode (syntax) according to dropdown
        scope.$watch('syntax', function(oldValue, newValue) {
          myCodeMirror.setOption('mode', scope.syntax);
        });

        // Set the codemirror value to the scope
        myCodeMirror.on('change', function(e) {
          $timeout(function() {
            scope.container.content = myCodeMirror.getValue();
          }, 300);
        });

      }
    };
  }
]);