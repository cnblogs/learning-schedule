import Swal from 'sweetalert2';
import { whereIp } from './ip.service';

const newLine = '%0A';
export function swalFailure(warmText: string, subject: string, body = ''): Promise<boolean> {
  if (typeof window !== 'undefined') {
    body += `${newLine} msg: ${warmText}
             ${newLine} userAgent: ${navigator.userAgent}
             ${newLine} url: ${encodeURIComponent(location.href)}`;
    return Swal.fire({
      title: '很抱歉，遇到了错误',
      text: warmText,
      type: 'error',
      confirmButtonText: '邮件联系博客园',
      showCancelButton: true,
      cancelButtonText: '忽略',
      focusCancel: true
    }).then(will => {
      if (will.value) {
        whereIp().subscribe(res => {
          body = body + `${newLine} ip: ${res}`;
          window.location.href = `mailto:contact@cnblogs.com?subject=${subject}&body=${body}`;
        });
        return false;
      } else {
        return true;
      }
    });
  }
}
