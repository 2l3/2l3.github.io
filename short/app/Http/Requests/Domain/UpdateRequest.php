<?php

namespace App\Http\Requests\Domain;

use Illuminate\Foundation\Http\FormRequest;

class UpdateRequest extends FormRequest
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
            'domain' => ['required', 'string', 'url'],
            'redirect' => ['nullable', 'url'],
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
            'domain.required' => __('The domain field is required'),
            'domain.string' => __('The domain must be a string'),
            'domain.url' => __('The domain format is invalid'),

            'redirect.url' => __('The redirect format is invalid'),
        ];
    }
}
