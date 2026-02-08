import { HttpErrorResponse, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let message = 'An error occurred';

      if (error.status === 0) {
        message = 'Server is unreachable. Please check your connection.';
      } else if (error.status === 401) {
        message = 'Session expired. Please login again.';
      } else if (error.status === 403) {
        message = 'You do not have permission.';
      } else if (error.status === 404) {
        message = 'Resource not found.';
      } else if (error.status >= 500) {
        message = 'Server error. Please try again later.';
      } else if (error.error?.message) {
        message = error.error.message;
      }

      return throwError(() => ({ ...error, message }));
    }),
  );
};
