<?php

namespace App\Http\Requests\Link;

use Illuminate\Foundation\Http\FormRequest;

class StoreRequest extends FormRequest
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
            'url' => ['url', 'required'],
            'code' => ['unique:links,code', 'nullable', 'min:5', 'max:50'],
            'type' => ['integer'],
            'title' => ['max:255'],
            'archived' => ['boolean'],
            'disabled' => ['boolean'],
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
            'url.url' => __('The url format is invalid'),
            'url.required' => __('The url field is required'),

            'code.unique' => __('The code has already been taken'),
            'code.min' => __('The code must be at least :min characters', ['min' => 5]),
            'code.max' => __('The code may not be greater than :max characters', ['max' => 255]),

            'type.integer' => __('The type must be an integer'),

            'title.max' => __('The title may not be greater than :max characters', ['max' => 255]),

            'archived.boolean' => __('The archived field must be true or false'),

            'disabled.boolean' => __('The archived field must be true or false'),
        ];
    }
}
