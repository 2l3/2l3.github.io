<?php

namespace App\Notifications;

use Illuminate\Bus\Queueable;
use Illuminate\Notifications\Messages\MailMessage;
use Illuminate\Notifications\Notification;
use stdClass;

class EmailVerify extends Notification
{
    use Queueable;

    /**
     * Email variables.
     *
     * @var stdClass
     */
    public $notificationData;

    /**
     * Create a new notification instance.
     *
     * ResetPassword constructor.
     * @param  stdClass  $objNotificationData
     */
    public function __construct(stdClass $objNotificationData)
    {
        $this->notificationData = $objNotificationData;
    }

    /**
     * Get the notification's delivery channels.
     *
     * @param  mixed  $notifiable
     * @return array
     */
    public function via($notifiable)
    {
        return ['mail'];
    }

    /**
     * Get the mail representation of the notification.
     *
     * @param  mixed  $notifiable
     * @return MailMessage
     */
    public function toMail($notifiable)
    {
        return (new MailMessage)
            ->success()
            ->subject(__('Verify email address'))
            ->greeting(__('Hi').' '.$this->notificationData->user->name.',')
            ->line(__('Please click the button below to verify your email address').'.')
            ->action(__('Verify email address'), url('/auth/verify/'.$this->notificationData->token).'?email='.urlencode($notifiable->email))
            ->line(__('If you have not registered yourself, someone has used this email address, we recommend that you contact the administrators').'.');
    }
}
