<?php

use App\Models\Setting;
use Illuminate\Database\Seeder;

class SettingsTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        if (Setting::where('key', 'app_name')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_name';
            $setting->value = env('APP_NAME', 'Dashboard');
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'app_description')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_description';
            $setting->value = 'A simple & powerful url shortener';
            $setting->save();
        }
        if (Setting::where('key', 'app_keywords')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_keywords';
            $setting->value = 'dashboard,laravel,vue,spa';
            $setting->save();
        }
        if (Setting::where('key', 'app_icon')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_icon';
            $setting->value = 'default';
            $setting->save();
        }
        if (Setting::where('key', 'app_background')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_background';
            $setting->value = 'default';
            $setting->save();
        }
        if (Setting::where('key', 'app_register')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_register';
            $setting->value = false;
            $setting->save();
        }
        if (Setting::where('key', 'app_timezone')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_timezone';
            $setting->value = env('APP_TIMEZONE', 'UTC');
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'app_locale')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_locale';
            $setting->value = env('APP_LOCALE', 'en');
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'app_date_locale')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_date_locale';
            $setting->value = 'en';
            $setting->save();
        }
        if (Setting::where('key', 'app_date_format')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_date_format';
            $setting->value = 'L';
            $setting->save();
        }
        if (Setting::where('key', 'default_role')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'default_role';
            $setting->value = 2;
            $setting->save();
        }
        if (Setting::where('key', 'unverified_user_alert')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'unverified_user_alert';
            $setting->value = false;
            $setting->save();
        }
        if (Setting::where('key', 'mail_mailer')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mail_mailer';
            $setting->value = env('MAIL_MAILER', 'log');
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mail_host')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mail_host';
            $setting->value = env('MAIL_HOST', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mail_port')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mail_port';
            $setting->value = env('MAIL_PORT', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mail_username')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mail_username';
            $setting->value = env('MAIL_USERNAME', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mail_password')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mail_password';
            $setting->value = env('MAIL_PASSWORD', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mail_encryption')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mail_encryption';
            $setting->value = env('MAIL_ENCRYPTION', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mail_from_address')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mail_from_address';
            $setting->value = env('MAIL_FROM_ADDRESS', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mail_from_name')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mail_from_name';
            $setting->value = env('MAIL_FROM_NAME', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mailgun_domain')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mailgun_domain';
            $setting->value = env('MAILGUN_DOMAIN', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mailgun_secret')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mailgun_secret';
            $setting->value = env('MAILGUN_SECRET', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'mailgun_endpoint')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'mailgun_endpoint';
            $setting->value = env('MAILGUN_ENDPOINT', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'postmark_token')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'postmark_token';
            $setting->value = env('POSTMARK_TOKEN', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'aws_access_key_id')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'aws_access_key_id';
            $setting->value = env('AWS_ACCESS_KEY_ID', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'aws_secret_access_key')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'aws_secret_access_key';
            $setting->value = env('AWS_SECRET_ACCESS_KEY', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'aws_default_region')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'aws_default_region';
            $setting->value = env('AWS_DEFAULT_REGION', null);
            $setting->is_env = true;
            $setting->save();
        }
        if (Setting::where('key', 'app_domain')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'app_domain';
            $setting->value = null;
            $setting->save();
        }
        if (Setting::where('key', 'only_primary_domain')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'only_primary_domain';
            $setting->value = false;
            $setting->save();
        }
        if (Setting::where('key', 'homepage_enabled')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'homepage_enabled';
            $setting->value = true;
            $setting->save();
        }
        if (Setting::where('key', 'homepage_description')->count() == 0) {
            $setting = new Setting();
            $setting->key = 'homepage_description';
            $setting->value = 'One link, a thousand numbers, analyze your visits.';
            $setting->save();
        }
    }
}
