<?php

namespace App\Http\Requests\Account;

use Illuminate\Foundation\Http\FormRequest;

class PasswordRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        return true;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        return [
            'current_password' => 'required',
            'password' => 'required|min:6|confirmed',
        ];
    }

    /**
     * Get custom messages for validator errors.
     *
     * @return array
     */
    public function messages()
    {
        return [
            'current_password.required' => __('The current password field is required'),
            'password.required' => __('The password field is required'),
            'password.min' => __('The password must be at least :min characters', ['min' => 6]),
            'password.confirmed' => __('The password confirmation does not match'),
        ];
    }
}
